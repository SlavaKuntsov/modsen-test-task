import { observer } from 'mobx-react-lite';
import { eventStore } from '../../utils/store/eventsStore';
import { userStore } from '../../utils/store/userStore';
// import { useUserStore } from '../../utils/store/UserStoreContext';

const Profile = observer(() => {
	// const userStore = useUserStore();

	const { logout, user } = userStore;
	const { resetStore } = eventStore;

	const handleLogout = async () => {
		await logout();
		resetStore();
	};

	return (
		<div className='flex items-center gap-4'>
			{user?.email}

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

export default Profile;
