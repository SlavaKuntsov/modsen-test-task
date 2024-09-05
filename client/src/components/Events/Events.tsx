import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react';
import { useEventItem } from '../../hooks/useEventItem';
import { getEvents } from '../../utils/api/eventsApi';
import { eventStore } from '../../utils/store/eventsStore';
import { getAccessToken } from '../../utils/tokens';
import Loader from '../Loader';
import EventListItem from './EventListItem';
import SelectedEventItem from './SelectedEventItem';

const Events = observer(() => {
	const { events, setEvents, isEventLoading, selectedEvent, searchingEvent } =
		eventStore;

	// const [accessToken, setAccessToken] = useState<string | null>('');

	// useEffect(() => {
		// setAccessToken(getAccessToken());
	// }, []);

	useEffect(() => {
		const fetchEvents = async () => {
			console.log('fetchEvents void');
			if (events) return;
			const data = await getEvents();
			setEvents(data);
		};

		fetchEvents();
	}, [events, setEvents]);

	const { isLoading, eventItem } = useEventItem(selectedEvent);

	const filteredEvents = searchingEvent
		? events!.filter(event =>
				event.title.toLowerCase().includes(searchingEvent.toLowerCase())
			)
		: events;

	return (
		<section className='w-full h-full px-10'>
			<div className='bg-white w-full h-full flex flex-row rounded-md border-[1px] border-solid border-zinc-200'>
				<div className='flex flex-col justify-start items-start  border-r-[1px] border-solid border-zinc-200 min-w-80 max-w-80'>
					{isEventLoading ? (
						filteredEvents!.length > 0 ? (
							filteredEvents?.map((item, id) => {
								return <EventListItem item={item} key={id} />;
							})
						) : (
							<div className='w-full h-full flex items-center justify-center text-center'>
								<h3 className='text-lg font-medium '>
									Событий с таким названием <br /> не найдено...
								</h3>
							</div>
						)
					) : (
						<Loader size='medium' />
					)}
				</div>
				<>
					{!eventItem ? (
						<>empty event</>
					) : !isLoading ? (
						<SelectedEventItem item={eventItem} />
					) : (
						<Loader size='medium' />
					)}
				</>
			</div>
		</section>
	);
});

export default Events;
