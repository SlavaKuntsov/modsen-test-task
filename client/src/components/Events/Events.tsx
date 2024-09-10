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
		setSelectEvent,
	} = eventStore;

	console.log('selectedEvent: ', selectedEvent);

	const [prevPage, setPrevPage] = useState<IEventsFetch>(fetch);

	const { user } = userStore;

	const { isLoading, eventItem, refetch, clearEventData } =
		useEventItem(selectedEvent);

	clearEventData();

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

		resetStore();
		// refreshEvents();
		setPrevPage(fetch);

		if (fetch === IEventsFetch.AllEvents) {
			fetchEvents();
		} else if (fetch === IEventsFetch.UserEvents) {
			fetchParticipant();
		}
	}, [fetch, user?.id, prevPage, resetStore, setEvents]);

	const filteredEvents = searchingEvent
		? events!.filter(event =>
				event.title.toLowerCase().includes(searchingEvent.toLowerCase())
			)
		: events;

	useEffect(() => {
		if (filteredEvents && filteredEvents?.length < 1)
			setSelectEvent(selectedEvent);
	}, [filteredEvents, setSelectEvent]);

	const refreshEvents = async () => {
		console.log('refreshEvents------------: ');

		if (fetch === IEventsFetch.AllEvents) {
			const data = await getEvents();
			setEvents(data);
		} else if (fetch === IEventsFetch.UserEvents) {
			const data = await getEventParticipant(user?.id);
			setEvents(data);
		}

		clearEventData();
		await refetch();
	};

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
					{!isLoading ? (
						eventItem ? (
							<SelectedEventItem
								item={eventItem!}
								fetch={fetch}
								refreshEvents={refreshEvents}
							/>
						) : (
							<div className='flex items-center justify-center w-full h-full'>
								<h1 className='text-center text-xl'>Событие не выбрано :(</h1>
							</div>
						)
					) : (
						<Loader size='medium' />
					)}
				</>
			</div>
		</section>
	);
});

export default Events;
