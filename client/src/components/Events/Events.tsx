import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react';
import { useEventItem } from '../../hooks/useEventItem';
import { getEventParticipant, getEvents } from '../../utils/api/eventsApi';
import { eventStore } from '../../utils/store/eventsStore';
import { userStore } from '../../utils/store/userStore';
import { IEventsFetch } from '../../utils/types';
import Loader from '../Loader';
import EventListItem from './EventListItem';
import SelectedEventItem from './SelectedEventItem';

const Events = observer(({ fetch }: { fetch: IEventsFetch }) => {
	const {
		events,
		setEvents,
		isEventLoading,
		selectedEvent,
		searchingEvent,
		resetStore,
	} = eventStore;
	const { user } = userStore;
	const [prevPage, setPrevPage] = useState<IEventsFetch>(fetch);

	// const [accessToken, setAccessToken] = useState<string | null>('');

	// useEffect(() => {
	// setAccessToken(getAccessToken());
	// }, []);

	// setPrevPage(fetch);

	useEffect(() => {
		const fetchEvents = async () => {
			console.log('fetchEvents void');
			const data = await getEvents();
			console.log('data all: ', data);
			setEvents(data);
		};

		const fetchParticipant = async () => {
			console.log('fetchParticipant void');
			const data = await getEventParticipant(user?.id);
			console.log('data participant: ', data);
			setEvents(data);
		};

		console.log('prevPage: ', prevPage);
		console.log('fetch: ', fetch);
		if (prevPage !== fetch) {
			resetStore();
			setPrevPage(fetch);
		}
		if (fetch === IEventsFetch.AllEvents) {
			fetchEvents();
		} else if (fetch === IEventsFetch.UserEvents) {
			fetchParticipant();
		}
	}, [fetch, user?.id, prevPage, resetStore, setEvents]);

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
								{fetch === IEventsFetch.UserEvents ? (
									<h3 className='text-lg font-medium p-2'>
										Зарегистрированных на Вас событий не найдено...
									</h3>
								) : (
									<h3 className='text-lg font-medium p-2'>
										Событий с таким названием <br /> не найдено...
									</h3>
								)}
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
						<SelectedEventItem item={eventItem} fetch={fetch} />
					) : (
						<Loader size='medium' />
					)}
				</>
			</div>
		</section>
	);
});

export default Events;
