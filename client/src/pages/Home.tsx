import { Link } from 'react-router-dom';
import { userStore } from '../utils/store/userStore';

export default function Home() {
	document.title = 'Home';

	const { logout } = userStore;

	return (
		<div className='flex flex-col items-center'>
			<h1 className='text-3xl'>Hello Modsen!</h1>
			<Link to='/login'>
				<button
					onClick={async () => await logout()}
					type='button'
					className='p-2 bg-zinc-900 text-zinc-50 self-start mt-2'
				>
					Выйти
				</button>
			</Link>
		</div>
	);
}
