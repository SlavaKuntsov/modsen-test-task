import { observer } from 'mobx-react-lite';
import { useLayoutEffect, useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import Loader from './components/Loader';
import {
	AuthGuard,
	AuthRoleGuard,
	UnAuthGuard,
} from './components/Routes/Guards';
import Layout from './layouts/Layout';
import LayoutContainer from './layouts/LayoutContainer';
import Login from './pages/Auth/Login';
import Registration from './pages/Auth/Registration';
import Admin from './pages/Main/Admin';
import Home from './pages/Main/Home';
import Participant from './pages/Main/Participant';
import NotFoundPage from './pages/NotFoundPage';
import { checkAccessToken } from './utils/api/authApi';
import { userStore } from './utils/store/userStore';
import { IUserRole } from './utils/types';

const App = observer(() => {
	const { user, setUser, isAuth, setAuth, isAuth2, setAuth2 } = userStore;
	const [isLoading, setIsLoading] = useState<boolean>(true);

	useLayoutEffect(() => {
		const fetchUserData = async () => {
			if (!isAuth2) {
				const userData = await checkAccessToken();
				if (userData) {
					setUser(userData);
					setAuth2(true);
				} else {
					setAuth2(false);
					setAuth(false);
				}
			}
			setIsLoading(false);
		};

		fetchUserData();
	}, [isAuth, setAuth, isAuth2, setAuth2]);

	if (isLoading) {
		return <Loader size='large' />;
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
				{/* Домашняя страница */}
				<Route
					index
					path='/'
					element={<AuthGuard user={user} component={<Home />} />}
				/>

				{/* Страница для участников */}
				<Route
					path='/participant'
					element={
						<AuthRoleGuard
							user={user}
							role={IUserRole.User}
							component={<Participant />}
						/>
					}
				/>

				{/* Страница для администраторов */}
				<Route
					path='/admin'
					element={
						<AuthRoleGuard
							user={user}
							role={IUserRole.Admin}
							component={<Admin />}
						/>
					}
				/>
			</Route>

			{/* Авторизация */}
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

			{/* Страница не найдена */}
			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	);
});

export default App;
