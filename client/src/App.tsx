import { observer } from 'mobx-react-lite';
import { useLayoutEffect, useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import { AuthGuard, UnAuthGuard } from './components/Routes/Guards';
import Layout from './layouts/Layout';
import Login from './pages/Auth/Login';
import Registration from './pages/Auth/Registration';
import Home from './pages/Home';
import NotFoundPage from './pages/NotFoundPage';
import { checkAccessToken } from './utils/api/authApi';
import { userStore } from './utils/store/userStore';

const App = observer(() => {
	// const navigate = useNavigate();

	const { user, isLoggedIn, setUser, isAuth, setAuth, isAuth2, setAuth2 } =
		userStore;
	const [isLoading, setIsLoading] = useState<boolean>(true);

	useLayoutEffect(() => {
		const fetchUserData = async () => {
			// Проверяем, авторизован ли пользователь
			console.log('isAuth: ', isAuth);
			if (!isAuth2) {
				const userData = await checkAccessToken();
				console.log('userData: ', userData);
				if (userData) {
					setUser(userData);
					setAuth2(true);
					// redirectTo('/main');
				} else {
					// redirectTo('/login');
				}
			} else {
				console.log('to /main');
				// navigate('/main')
			}
			setIsLoading(false);
		};

		fetchUserData();
	}, [isAuth, setAuth, isAuth2, setAuth2]);

	if (isLoading) {
		return <h1>Загрузка...</h1>; // Отображаем загрузку пока проверяем аутентификацию
	}
	return (
		<Routes>
			<Route path='/' element={<Layout />}>
				<Route
					index
					path='/'
					element={<AuthGuard user={user} component={<Home />} />}
				/>
				<Route
					path='/login'
					element={<UnAuthGuard user={user} component={<Login />} />}
				/>
				<Route
					path='/registration'
					element={<UnAuthGuard user={user} component={<Registration />} />}
				/>
				<Route path='*' element={<NotFoundPage />} />
			</Route>
		</Routes>
	);
});

export default App;
