import { Radio, RadioChangeEvent, Switch } from 'antd';
import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react';
import { useEventItem } from '../../hooks/useEventItem';
import { getEventParticipant, getEvents } from '../../utils/api/eventsApi';
import { eventStore } from '../../utils/store/eventsStore';
import { userStore } from '../../utils/store/userStore';
import { IEventsFetch, SortType } from '../../utils/types';
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

	const [sort, setSort] = useState<SortType>('title');
	const [reverseOrder, setReverseOrder] = useState<boolean>(false);

	const handleSortChange = (e: RadioChangeEvent) => {
		setSort(e.target.value);
	};

	// Проверка на null перед сортировкой
	const sortEvents = (events: (typeof eventStore)['events'] | null) => {
		if (!events) return [];

		const sortedEvents = [...events];

		switch (sort) {
			case 'title':
				sortedEvents.sort((a, b) => a.title.localeCompare(b.title));
				break;
			case 'date':
				sortedEvents.sort(
					(a, b) =>
						new Date(a.eventDateTime).getTime() -
						new Date(b.eventDateTime).getTime()
				);
				break;
			case 'participantCount':
				sortedEvents.sort((a, b) => b.participantsCount - a.participantsCount);
				break;
			default:
				break;
		}

		return reverseOrder ? sortedEvents.reverse() : sortedEvents;
	};

	const removeEventById = (id: string) => {
		if (events) {
			setEvents(events.filter(event => event.id !== id));
		} else {
			setEvents(null);
		}
	};

	const onSwitchChange = (checked: boolean) => {
		setReverseOrder(checked);
	};

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
		setPrevPage(fetch);

		if (fetch === IEventsFetch.AllEvents) {
			fetchEvents();
		} else if (fetch === IEventsFetch.UserEvents) {
			fetchParticipant();
		}
	}, [fetch, user?.id, prevPage, resetStore, setEvents]);

	const filteredEvents = searchingEvent
		? events?.filter(event =>
				event.title.toLowerCase().includes(searchingEvent.toLowerCase())
			)
		: events;

	// Применяем сортировку только если filteredEvents не равен null
	const sortedEvents = sortEvents(filteredEvents ?? []);

	useEffect(() => {
		if (filteredEvents && filteredEvents.length < 1) {
			setSelectEvent(selectedEvent);
		}
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
			<div className='bg-white min-h-full max-h-[80vh] flex flex-row rounded-md border-[1px] border-solid border-zinc-200'>
				<div className='flex flex-col justify-start items-start  border-r-[1px] border-solid border-zinc-200 min-w-96 max-w-96 overflow-y-auto overflow-x-hidden'>
					{isEventLoading ? (
						sortedEvents!.length > 0 ? (
							<>
								<div className='w-full flex flex-col items-center justify-center py-2 border-b-[1px] border-solid border-zinc-200 gap-2'>
									<Radio.Group
										value={sort}
										onChange={handleSortChange}
										size='middle'
									>
										<Radio.Button value='title'>Title</Radio.Button>
										<Radio.Button value='date'>Date</Radio.Button>
										<Radio.Button value='participantCount'>
											Participant Count
										</Radio.Button>
									</Radio.Group>
									<div className='flex items-center justify-center gap-2'>
										<p className='text-lg'>В обратном порядке</p>
										<Switch
											defaultChecked={reverseOrder}
											onChange={onSwitchChange}
										/>
									</div>
								</div>
								{sortedEvents?.map((item, id) => {
									return <EventListItem item={item} key={id} />;
								})}
							</>
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
						<Loader size='medium' className='h-full' />
					)}
				</div>
				<>
					{!isLoading ? (
						eventItem ? (
							<SelectedEventItem
								item={eventItem!}
								fetch={fetch}
								refreshEvents={refreshEvents}
								onDelete={removeEventById}
							/>
						) : (
							<div className='flex items-center justify-center w-full'>
								<h1 className='text-center text-xl'>Событие не выбрано</h1>
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
