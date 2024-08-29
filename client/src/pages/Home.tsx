import { Link } from 'react-router-dom';

export default function Home() {
	document.title = 'Home';

	return (
		<div className='flex flex-col items-center'>
			<h1 className='text-3xl'>Hello Modsen!</h1>
			<Link to='login'>
				<button
					onClick={() => localStorage.setItem('user', JSON.stringify(null))}
					type='button'
					className='p-2 bg-zinc-900 text-zinc-50 self-start mt-2'
				>
					Выйти
				</button>
			</Link>
		</div>
	);
}
