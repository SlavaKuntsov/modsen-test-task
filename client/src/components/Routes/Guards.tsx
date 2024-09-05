import { Navigate } from 'react-router-dom';
import { IUser, IUserRole } from '../../utils/types';

export function AuthGuard({
	component,
	user,
}: {
	component: JSX.Element;
	user: IUser | null;
}) {
	// Если пользователь не авторизован, перенаправляем на страницу логина
	if (!user) {
		console.log('AuthGuard: Redirecting to login');
		return <Navigate to='/auth/login' replace />;
	}

	// Если все условия выполнены, возвращаем компонент
	return component;
}

export function AuthRoleGuard({
	component,
	user,
	role, // Добавляем ожидание роли
}: {
	component: JSX.Element;
	user: IUser | null;
	role: IUserRole.Admin | IUserRole.User; // Ограничиваем доступные роли
}) {
	// Если пользователь не авторизован, перенаправляем на страницу логина
	if (!user) {
		console.log('AuthGuard: Redirecting to login');
		return <Navigate to='/auth/login' replace />;
	}

	// Если пользователь авторизован, но его роль не соответствует требуемой
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

	// Если все условия выполнены, возвращаем компонент
	return component;
}

export function UnAuthGuard({
	component,
	user,
}: {
	component: JSX.Element;
	user: IUser | null;
}) {
	// if (user?.role === IUserRole.User) {
	// 	console.log('UnAuthGuard: Redirecting to main participant page');
	// 	return <Navigate to='/participant' replace />;
	// }

	// if (user?.role === IUserRole.Admin) {
	// 	console.log('UnAuthGuard: Redirecting to main admin page');
	// 	return <Navigate to='/admin' replace />;
	// }

	if (user) {
		console.log('UnAuthGuard to main');
		return <Navigate to='/' replace />;
	}

	return component;
}
