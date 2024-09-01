import { Outlet } from 'react-router-dom';

export default function Layout() {
	return (
		<div className='bg-slate-50 h-full p-4'>
			<div className='container mx-auto flex items-center justify-center h-full w-full'>
				{<Outlet />}
			</div>
		</div>
	);
}
