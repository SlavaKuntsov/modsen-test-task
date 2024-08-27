import { Navigate } from 'react-router-dom';

export default function UnAuthGuard({ component, user }) {
	console.log('user: ', user);

	if (user) {
		return <Navigate to='/' replace />;
	}

	return <>{component}</>;
}
