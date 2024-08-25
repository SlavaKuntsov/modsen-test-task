import { Navigate } from 'react-router-dom';

export default function AuthGuard({ component, user }) {
	console.log('user: ', user);

	if (!user) {
		return <Navigate to='/login' replace />;
	}

	return <>{component}</>;
}
