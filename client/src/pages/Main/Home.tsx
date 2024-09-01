import Events from '../../components/Events';

export default function Home() {
	document.title = 'Home';

	

	return (
		<div className='flex flex-col items-center w-full h-full gap-5'>
			<h1 className='text-3xl'>Hello Modsen!</h1>

			<Events />

			{/* <div className='flex gap-4'>
				<button
					onClick={async () => await getEvents()}
					className='p-2 bg-zinc-900 text-zinc-50 self-start mt-2'
				>
					GetEvents
				</button>

				<button
					onClick={async () => await getEventsAdmin()}
					className='p-2 bg-zinc-900 text-zinc-50 self-start mt-2'
				>
					GetEventsAdmin
				</button>
			</div> */}
		</div>
	);
}
