import classNames from 'classnames';
import { Link, useLocation } from 'react-router-dom';
import { userStore } from '../../utils/store/userStore';
import { IRoute, IUserRole } from '../../utils/types';
import NavProfile from './NavProfile';

export default function Nav() {
	const location = useLocation(); // Получаем текущее местоположение
	const { user } = userStore;

	const routes: Array<IRoute> = [
		{ name: 'Все события', path: '/', role: IUserRole.All },
		{ name: 'Мои события', path: '/participant', role: IUserRole.User },
		{ name: 'Админ панель', path: '/admin', role: IUserRole.Admin },
		// name: '', path: '/events'
	];

	return (
		<nav className='w-full grid grid-cols-3 gap-4 items-center mb-4 bg-[#1e293b] px-4 py-3 rounded-md text-zinc-50'>
			<div>
				<h1 className='font-semibold text-2xl'>Events</h1>
			</div>
			<div className='flex flex-row gap-6 justify-center'>
				{routes
					.filter(item => {
						return item.role === IUserRole.All || user?.role === item.role;
					})
					.map((item, id) => {
						return (
							<Link to={item.path} key={id}>
								<button
									className={classNames('text-lg cursor-pointer', {
										'underline underline-offset-[5px] decoration-[.5px]':
											location.pathname === item.path,
									})}
								>
									{item.name}
								</button>
							</Link>
						);
					})}
			</div>
			<div className='grid justify-items-end'>
				<NavProfile />
			</div>
		</nav>
	);
}
