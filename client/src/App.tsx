import { observer } from 'mobx-react-lite';
import { useEffect, useLayoutEffect, useState } from 'react';
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
import Profile from './pages/Main/Profile';
import NotFoundPage from './pages/NotFoundPage';
import { checkAccessToken } from './utils/api/authApi';
import { eventStore } from './utils/store/eventsStore';
import { userStore } from './utils/store/userStore';
import { IUserRole } from './utils/types';

const App = observer(() => {
	const { user, setUser, isAuth, setAuth, isAuth2, setAuth2 } = userStore;
	const { resetStore, events, selectedEvent } = eventStore;
	const [isLoading, setIsLoading] = useState<boolean>(true);

	useEffect(() => {
		resetStore();
	}, [resetStore]);

	useLayoutEffect(() => {
		const fetchUserData = async () => {
			console.log(123)
			if (!isAuth2) {
				const userData = await checkAccessToken();
				console.log('userData: ', userData);
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
		return <Loader size='large' className='h-full'/>;
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

				{/* Список событий где зарегистрирован участник */}
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

				<Route
					path='/profile'
					element={
						<AuthRoleGuard
							user={user}
							role={IUserRole.User}
							component={<Profile />}
						/>
					}
				/>

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
