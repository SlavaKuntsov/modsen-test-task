import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
import { eventStore } from '../../utils/store/eventsStore';
import { userStore } from '../../utils/store/userStore';

const NavProfile = observer(() => {
	const { logout, user } = userStore;
	const { resetStore } = eventStore;

	const handleLogout = async () => {
		await logout();
		resetStore();
	};

	return (
		<div className='flex items-center gap-4'>
			<Link to='/profile' className='text-lg'>
				{user?.email}
			</Link>

			<button
				onClick={handleLogout}
				type='button'
				className='px-2 py-1 bg-zinc-900 text-zinc-50 self-start'
			>
				Выйти
			</button>
		</div>
	);
});

export default NavProfile;
