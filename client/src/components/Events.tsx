import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { useEventItem } from '../hooks/useEventItem';
import { getEvents } from '../utils/api/eventsApi';
import { eventStore } from '../utils/store/eventsStore';

const Events = observer(() => {
	const { events, setEvents, isEventLoading, selectedEvent, setSelectEvent } =
		eventStore;

	useEffect(() => {
		const fetchEvents = async () => {
			console.log('fetchEvents: ');

			if (events) return;

			const data = await getEvents();

			console.log('events data: ', data);
			setEvents(data);
		};

		fetchEvents();
	}, [events, setEvents]);

	const { isLoading, error, eventItem } = useEventItem(selectedEvent);

	// if (isLoading) return 'Loading...'; // Замените isPending на isLoading

	// if (error) return 'An error has occurred: ' + (error as Error).message;

	return (
		<section className='w-full h-full px-10'>
			<div className='bg-white w-full h-full flex flex-row rounded-md border-[1px] border-solid border-zinc-200'>
				<div className='flex flex-col justify-start items-start  border-r-[1px] border-solid border-zinc-200'>
					{isEventLoading ? (
						events!.map((item, id) => {
							return (
								<button
									onClick={() => setSelectEvent(item.id)}
									className='p-4'
									key={id}
								>
									{item.title}
								</button>
							);
						})
					) : (
						<>load</>
					)}
				</div>
				<div className='w-full p-4'>
					{!eventItem ? (
						<>empty event</>
					) : !isLoading ? (
						<ul>
							<li>{eventItem!.id}</li>
							<li>{eventItem!.title}</li>
							<li>{eventItem!.category}</li>
							<li>{eventItem!.eventDateTime}</li>
						</ul>
					) : (
						<>loading</>
					)}
				</div>
			</div>
		</section>
	);
});

export default Events;
