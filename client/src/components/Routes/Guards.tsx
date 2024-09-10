import { Navigate } from 'react-router-dom';
import { IUser, IUserRole } from '../../utils/types';

export function AuthGuard({
	component,
	user,
}: {
	component: JSX.Element;
	user: IUser | null;
}) {
	if (!user) {
		console.log('AuthGuard: Redirecting to login');
		return <Navigate to='/auth/login' replace />;
	}

	return component;
}

export function AuthRoleGuard({
	component,
	user,
	role,
}: {
	component: JSX.Element;
	user: IUser | null;
	role: IUserRole.Admin | IUserRole.User;
}) {
	if (!user) {
		console.log('AuthGuard: Redirecting to login');
		return <Navigate to='/auth/login' replace />;
	}

	if (user.role !== role) {
		console.log(
			`AuthGuard: Redirecting due to role mismatch, user role ${user.role} does not match ${role}`
		);
		return (
			<Navigate
				to={user.role === IUserRole.Admin ? '/admin' : '/participant'}
				replace
			/>
		);
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
		console.log('UnAuthGuard to main');
		return <Navigate to='/' replace />;
	}

	return component;
}
