import { useEffect, useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import AuthGuard from './components/Routes/AuthGuard';
import UnAuthGuard from './components/Routes/UnAuthGuard';
import UseGlobalNavigate, { redirectTo } from './hooks/UseGlobalNavigate';
import Layout from './layouts/Layout';
import Login from './pages/Auth/Login';
import Registration from './pages/Auth/Registration';
import Home from './pages/Home';
import NotFoundPage from './pages/NotFoundPage';
import { User } from './utils/api/userApi';
import { checkAccessToken } from './utils/api/authApi';

export default function App() {
	UseGlobalNavigate();

	const [user, setUser] = useState<User | null>(() => {
		const userData = localStorage.getItem('user');
		return userData ? (JSON.parse(userData) as User) : null;
	});

	checkAccessToken();

	useEffect(() => {
		console.log(user);

		if (user !== null) redirectTo('/');

		// else redirectTo('/login');
	}, [user]);

	return (
		<Routes>
			<Route path='/' element={<Layout />}>
				<Route index element={<AuthGuard user={user} component={<Home />} />} />
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
}
