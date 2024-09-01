import { observer } from 'mobx-react-lite';
import { useUserStore } from '../../utils/store/UserStoreContext';

const Profile = observer(() => {
	const userStore = useUserStore();

	const { user } = userStore;
	console.log('user profile: ', user);

	return <div>{user?.email}</div>;
});

export default Profile;
