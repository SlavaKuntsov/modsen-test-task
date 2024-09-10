import { QuestionCircleOutlined } from '@ant-design/icons';
import { DatePicker, Popconfirm } from 'antd';
import classNames from 'classnames';
import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
import { ErrorMessage, Field, FieldInputProps, Formik } from 'formik';
import { useState } from 'react';
import * as Yup from 'yup';
import {
	deleteEvent,
	eventRegistration,
	eventUnregistration,
	updateEvent,
} from '../../utils/api/eventsApi';
import { eventStore } from '../../utils/store/eventsStore';
import { userStore } from '../../utils/store/userStore';
import { IDelete, IEvent, IEventsFetch, IUserRole } from '../../utils/types';
import Button from '../Button';
import useCustomToast from '../Toast';

dayjs.extend(customParseFormat);

// Event validation schema using Yup
const EventSchema = Yup.object().shape({
	title: Yup.string().required('Title is required'),
	description: Yup.string().required('Description is required'),
	eventDateTime: Yup.string().required('Event date and time are required'),
	location: Yup.string().required('Location is required'),
	category: Yup.string().required('Category is required'),
	maxParticipants: Yup.number()
		.required('Max participants are required')
		.min(1, 'At least one participant is required'),
});

interface SelectedEventItemProps {
	item: IEvent;
	fetch: IEventsFetch;
	refreshEvents: () => Promise<void>;
	onDelete: (id: string) => void;
}

const SelectedEventItem = ({
	item,
	fetch,
	refreshEvents,
	onDelete
}: SelectedEventItemProps) => {
	console.log('item: ', item);

	const { user } = userStore;
	const { events, setEvents } = eventStore;
	const { removeEventById } = eventStore;

	const { showToast } = useCustomToast();

	const [file, setFile] = useState<File | null>(null);

	const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		if (event.target.files && event.target.files.length > 0) {
			setFile(event.target.files[0]);
		}
	};

	const [isChanged, setIsChanged] = useState<boolean>(false);
	console.log('isChanged: ', isChanged);

	const handleRegistration = async () => {
		try {
			const result = await eventRegistration({
				eventId: item.id,
				participantId: user!.id,
			});

			if (result === true) {
				refreshEvents();
				showToast({
					title: 'Успешно!',
					description: 'Вы зарегистрированы на это событие!',
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
				removeEventById(item.id);
				await refreshEvents();

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
			console.error('Ошибка при отписке от события:', error);
			showToast({
				title: 'Произошла ошибка при отмене регистрации!',
				status: 'error',
			});
		}
	};

	const handleUpdateEvent = async (values: IEvent) => {
		console.log('values: ', values);

		const base64data = file
			? await new Promise<string>((resolve, reject) => {
					const reader = new FileReader();
					reader.readAsDataURL(file!);
					reader.onloadend = () => {
						const result = reader.result as string;
						resolve(result.split(',')[1]);
					};
					reader.onerror = error => {
						reject(error);
					};
				})
			: item.image;
		try {
			const formattedEventDateTime = dayjs(values.eventDateTime).format(
				'DD-MM-YYYY'
			);

			const updatedEvent: IEvent = {
				id: item.id,
				title: values.title,
				description: values.description,
				location: values.location,
				category: values.category,
				maxParticipants: values.maxParticipants,
				eventDateTime: formattedEventDateTime,
				participantsCount: values.participantsCount,
				image: base64data,
			};

			const result = await updateEvent(updatedEvent);
			console.log('Event updated:', result);

			if (result) {
				setIsChanged(false);
				// resetStore()
				refreshEvents();
				showToast({
					title: 'Событие успешно обновлено!',
					status: 'success',
				});
			} else if (typeof result === 'string') {
				showToast({
					title: result,
					status: 'error',
				});
			}
		} catch (error) {
			console.error('Error updating event:', error);
			showToast({
				title: 'Произошла ошибка при обновлении события!',
				status: 'error',
			});
		}
	};

	const handleDelete = async () => {
		try {
			const eventData: IDelete = {
				id: item?.id,
			};

			const result = await deleteEvent(eventData);

			// Проверяем, что result является объектом IUser
			if (result) {
				showToast({
					title: 'Успешно!',
					status: 'success',
				});
				// refreshEvents();
				onDelete(item.id); 
			} else if (typeof result === 'string') {
				// Если result — это строка (ошибка), выводим сообщение об ошибке
				showToast({
					title: result,
					status: 'error',
				});
			}
		} catch (error) {
			console.error('Error deleting user:', error);
			showToast({
				title: 'An unexpected error occurred',
				status: 'error',
			});
		}
	};

	return (
		// <div className='w-full h-full'>
		<Formik
			initialValues={{
				title: item.title,
				description: item.description,
				eventDateTime: item.eventDateTime,
				location: item.location,
				category: item.category,
				maxParticipants: item.maxParticipants,
				participantsCount: item.participantsCount,
			}}
			enableReinitialize={true}
			validationSchema={EventSchema}
			onSubmit={(values, { setSubmitting }) => {
				handleUpdateEvent(values);
				setSubmitting(false);
			}}
		>
			{({ errors, touched, handleSubmit, setFieldValue }) => (
				<>
					<form
						onSubmit={handleSubmit}
						className='flex flex-col items-start gap-1 w-full overflow-y-auto overflow-x-hidden'
					>
						<div className='top-block w-full relative'>
							<img
								className='w-full h-96 border-b-[1px] border-solid border-zinc-200 bg-zinc-100'
								src={`data:image/jpeg;base64,${item.image}`}
								alt='event image'
							/>
							<div className='absolute left-0 bottom-0 flex items-center justify-between w-full p-3 px-20'>
								{!isChanged ? (
									<h1 className='bg-zinc-200 rounded-md px-5 py-2 font-semibold text-2xl'>
										{item.title}
									</h1>
								) : (
									// <div className=' flex flex-col gap-1'>
									<div>
										<Field
											name='title'
											placeholder='Title'
											className={classNames(
												'max-w-80 h-[36px] py-2 px-3 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500 text-xl',
												touched.title && errors.title
													? 'border-red-500'
													: 'border-gray-300'
											)}
										/>
										<ErrorMessage
											name='title'
											component='div'
											className='text-red-500 text-base'
										/>
									</div>
								)}
								<div className='flex flex-row gap-4 items-center justify-center mr-4'>
									{user?.role === IUserRole.User ? (
										fetch == IEventsFetch.UserEvents ? (
											<Button
												className='!bg-[#1e293b]'
												onClick={handleUnregistration}
												type='primary'
											>
												Отписаться
											</Button>
										) : (
											<Button
												onClick={handleRegistration}
												type='primary'
												className='!bg-[#1e293b]'
											>
												Зарегистрироваться
											</Button>
										)
									) : (
										<>
											{isChanged && (
												<Button
													htmlType='submit'
													// onClick={handleUpdateEvent}
													className='!bg-[#1e293b]'
													type='primary'
												>
													Сохранить
												</Button>
											)}
											<Button
												onClick={() => setIsChanged(prev => !prev)}
												className='!bg-[#1e293b]'
												type='primary'
											>
												Изменить
											</Button>
											<Popconfirm
												onConfirm={handleDelete}
												title='Удаление события'
												description='Вы уверены что хотите удалить событие?'
												icon={
													<QuestionCircleOutlined style={{ color: 'red' }} />
												}
											>
												<Button
													onClick={() => console.log('object')}
													className='!bg-red-500'
													type='primary'
												>
													Удалить
												</Button>
											</Popconfirm>
										</>
									)}
								</div>
							</div>
						</div>
						<div className='px-20 py-5 flex flex-col gap-10 items-start w-full h-full'>
							{isChanged && (
								<input
									type='file'
									onChange={handleFileChange}
									className='mt-2'
								/>
							)}
							{!isChanged ? (
								<h3 className='text-2xl text-zinc-700 font-medium'>
									{item.description}
								</h3>
							) : (
								<>
									<Field
										name='description'
										placeholder='Description'
										className={classNames(
											'w-80 h-[36px] py-2 px-3 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500 text-xl',
											touched.description && errors.description
												? 'border-red-500'
												: 'border-gray-300'
										)}
									/>
									<ErrorMessage
										name='description'
										component='span'
										className='text-red-500'
									/>
								</>
							)}
							<div className='flex flex-row items-center justify-evenly text-xl text-zinc-800 w-full bg-zinc-100 py-3 rounded-md'>
								<h3 className='text-nowrap flex flex-row items-center justify-center gap-2 font-medium'>
									Категория:{' '}
									{!isChanged ? (
										<p className='font-normal'>{item.category}</p>
									) : (
										<>
											<Field
												name='category'
												placeholder='Category'
												className={classNames(
													'w-36 py-1 px-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500 text-xl font-normal',
													touched.category && errors.category
														? 'border-red-500'
														: 'border-gray-300'
												)}
											/>
											<ErrorMessage
												name='category'
												component='span'
												className='text-red-500'
											/>
										</>
									)}
								</h3>
								<span className='w-3 h-3 rounded-full bg-zinc-500'></span>
								<h3 className='text-nowrap flex flex-row items-center justify-center gap-2 font-medium'>
									Локация:{' '}
									{!isChanged ? (
										<p className='font-normal'>{item.location}</p>
									) : (
										<>
											<Field
												name='location'
												placeholder='Location'
												className={classNames(
													'w-36 py-1 px-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500 text-xl font-normal',
													touched.location && errors.location
														? 'border-red-500'
														: 'border-gray-300'
												)}
											/>
											<ErrorMessage
												name='location'
												component='span'
												className='text-red-500'
											/>
										</>
									)}
								</h3>
								<span className='w-3 h-3 rounded-full bg-zinc-500'></span>
								<h3 className='text-nowrap flex flex-row items-center justify-center gap-2 font-medium'>
									Дата и Время:{' '}
									{!isChanged ? (
										<p className='font-normal'>
											{dayjs(item.eventDateTime).format('DD-MM-YYYY')}
										</p>
									) : (
										<>
											<Field name='eventDateTime'>
												{({
													field,
												}: {
													field: FieldInputProps<Date | null>;
												}) => (
													<DatePicker
														{...field}
														value={field.value ? dayjs(field.value) : null}
														onChange={date => {
															setFieldValue(
																'eventDateTime',
																date ? date.toDate() : null
															);
															console.log(date.toDate());
														}}
														format='DD-MM-YYYY'
														size='middle'
														className={classNames(
															'w-full h-[39px] mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
															{
																'border-red-500':
																	touched.eventDateTime && errors.eventDateTime,
																'border-green-500':
																	touched.eventDateTime &&
																	!errors.eventDateTime,
															}
														)}
													/>
												)}
											</Field>
											<ErrorMessage
												name='eventDateTime'
												component='span'
												className='text-red-500'
											/>
										</>
									)}
								</h3>
							</div>
							<h2 className='text-nowrap flex flex-row items-center justify-center gap-2 font-medium text-2xl text-zinc-700'>
								Максимальное число участников:{' '}
								{!isChanged ? (
									<p className='font-normal'>{item.maxParticipants}</p>
								) : (
									<>
										<Field
											name='maxParticipants'
											placeholder='MaxParticipants'
											className={classNames(
												'w-36 py-1 px-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500 text-xl font-normal',
												touched.maxParticipants && errors.maxParticipants
													? 'border-red-500'
													: 'border-gray-300'
											)}
										/>
										<ErrorMessage
											name='maxParticipants'
											component='span'
											className='text-red-500'
										/>
									</>
								)}
							</h2>
							<h2 className='text-2xl font-medium text-zinc-700 text-nowrap flex flex-row items-center justify-center gap-2'>
								Текущее число участников:{' '}
								<p className='font-normal'>{item.participantsCount}</p>
							</h2>
						</div>
					</form>
				</>
			)}
		</Formik>
		// </div>
	);
};

export default SelectedEventItem;
