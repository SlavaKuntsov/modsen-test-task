import { Navigate } from 'react-router-dom';
import { IUser } from '../../utils/types/types';

export function AuthGuard({
	component,
	user,
}: {
	component: JSX.Element;
	user: IUser | null;
}) {
	if (!user) {
		console.log('AuthGuard to login')
		return <Navigate to='/login' replace />;
	}
	return component;
}

export function UnAuthGuard({
	component,
	user,
}: {
	component: JSX.Element;
	user: IUser | null;
}) {
	if (user) {
		console.log('UnAuthGuard to main')
		return <Navigate to='/' replace />;
	}
	return component;
}
