import { Link } from 'react-router-dom';

export default function NotFoundPage() {
	return (
		<div className='flex flex-col gap-1'>
			<h1>Page Not Found!</h1>
			<Link to='/main' className='bg-red-100'>
				home
			</Link>
		</div>
	);
}
