import {
	eventRegistration,
	eventUnregistration,
} from '../../utils/api/eventsApi';
import { eventStore } from '../../utils/store/eventsStore';
import { userStore } from '../../utils/store/userStore';
import { IEvent, IEventsFetch, IUserRole } from '../../utils/types';
import Button from '../Button';
import useCustomToast from '../Toast';

interface SelectedEventItemProps {
	item: IEvent;
	fetch: IEventsFetch;
}

export default function SelectedEventItem({
	item,
	fetch,
}: SelectedEventItemProps) {
	const { user } = userStore;
	const { removeEventById, setSelectEvent } = eventStore;

	const { showToast } = useCustomToast();

	const handleRegistration = async () => {
		try {
			const result = await eventRegistration({
				eventId: item.id,
				participantId: user!.id,
			});

			if (result === true) {
				showToast({
					title: 'Успешно!',
					status: 'success',
				});
			} else if (typeof result === 'string') {
				showToast({
					title: result,
					status: 'error',
				});
			}
		} catch (error) {
			console.error('Error reg on event:', error);
			throw error;
		}
	};

	const handleUnregistration = async () => {
		try {
			const result = await eventUnregistration({
				eventId: item.id,
				participantId: user!.id,
			});

			if (result === true) {
				removeEventById(item.id); // Remove the event by ID
				setSelectEvent(null);
				showToast({
					title: 'Успешно!',
					description: 'Событие удалено из вашего списка!',
					status: 'success',
				});
			} else if (typeof result === 'string') {
				showToast({
					title: result,
					status: 'error',
				});
			}
		} catch (error) {
			console.error('Error reg on event:', error);
			throw error;
		}
	};

	return (
		<div className='w-full h-full'>
			<div className='relative'>
				<img
					className='w-full h-80 border-2 border-solid border-red-200'
					src=''
					alt='event image'
				/>
				<div className='absolute left-0 bottom-0 flex items-center justify-between w-full p-2'>
					<h1 className='bg-zinc-50 rounded-md px-4 py-2 font-semibold text-2xl ml-20'>
						{item.title}
					</h1>
					<div className='flex flex-row gap-4 items-center justify-center mr-4'>
						{user?.role === IUserRole.User ? (
							fetch == IEventsFetch.UserEvents ? (
								<Button onClick={handleUnregistration} type='primary'>
									Отписаться
								</Button>
							) : (
								<Button onClick={handleRegistration} type='primary'>
									Зарегистрироваться
								</Button>
							)
						) : (
							<Button type='primary'>Изменить</Button>
						)}
					</div>
				</div>
			</div>
			<div className='px-8 py-3'>
				<h3 className='text-lg'>Описание: {item.description}</h3>
			</div>
		</div>
	);
}
