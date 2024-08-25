import { useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import AuthGuard from './components/Routes/AuthGuard';
import UnAuthGuard from './components/Routes/UnAuthGuard';
import Layout from './layouts/Layout';
import Login from './pages/Auth/Login';
import Registration from './pages/Auth/Registration';
import Home from './pages/Home';
import NotFoundPage from './pages/NotFoundPage';

export default function App() {
	type User = {
		email: string;
		password: string;
	} | null;

	const [user, setUser] = useState<User>({
		email: 'slava',
		password: '1',
	});

	const [user2, setUser2] = useState<User>(null);

	return (
		<Routes>
			<Route path='/' element={<Layout />}>
				<Route
					index
					element={<AuthGuard user={user2} component={<Home />} />}
				/>
				<Route
					path='/login'
					element={<UnAuthGuard user={user2} component={<Login />} />}
				/>
				<Route
					path='/registration'
					element={<UnAuthGuard user={user2} component={<Registration />} />}
				/>
				<Route path='*' element={<NotFoundPage />} />
			</Route>
		</Routes>
	);
}
