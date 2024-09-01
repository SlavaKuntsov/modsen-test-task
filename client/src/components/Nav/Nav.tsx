import Profile from './Profile';

export default function Nav() {
	return (
		<section className='w-full grid grid-flow-col items-center mb-4'>
			<div>
				<h1 className='font-semibold text-2xl text-zinc-900'>Events</h1>
			</div>
			<div>w</div>
			<div className='grid justify-items-end'>
				<Profile />
			</div>
		</section>
	);
}
