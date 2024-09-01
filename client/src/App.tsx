import { observer } from 'mobx-react-lite';
import { useLayoutEffect, useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import { AuthGuard, UnAuthGuard } from './components/Routes/Guards';
import Layout from './layouts/Layout';
import LayoutContainer from './layouts/LayoutContainer';
import Login from './pages/Auth/Login';
import Registration from './pages/Auth/Registration';
import Home from './pages/Main/Home';
import Participant from './pages/Main/Participant';
import NotFoundPage from './pages/NotFoundPage';
import { checkAccessToken } from './utils/api/authApi';
import { userStore } from './utils/store/userStore';

const App = observer(() => {
	// const navigate = useNavigate();

	const { user, setUser, isAuth, setAuth, isAuth2, setAuth2 } = userStore;
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
				} else if (userData == null) {
					setAuth2(false);
					setAuth(false);
				}
			} else {
				console.log('to /main');
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
			<Route
				path='/'
				element={
					<Layout>
						<LayoutContainer />
					</Layout>
				}
			>
				<Route
					index
					path='/'
					element={<AuthGuard user={user} component={<Home />} />}
				/>
				<Route
					path='/participant'
					element={<AuthGuard user={user} component={<Participant />} />}
				/>
			</Route>
			<Route
				path='/auth'
				element={
					<Layout>
						<LayoutContainer isAuth={true} />
					</Layout>
				}
			>
				<Route
					path='login'
					element={<UnAuthGuard user={user} component={<Login />} />}
				/>
				<Route
					path='registration'
					element={<UnAuthGuard user={user} component={<Registration />} />}
				/>
			</Route>
			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	);
});

export default App;
