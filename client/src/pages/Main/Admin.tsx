import { DatePicker } from 'antd';
import classNames from 'classnames';
import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
import { ErrorMessage, Field, FieldInputProps, Formik } from 'formik';
import { useState } from 'react';
import * as Yup from 'yup';
import Button from '../../components/Button';
import useCustomToast from '../../components/Toast';
import { createEvent } from '../../utils/api/eventsApi';
import { IEvent } from '../../utils/types';

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

export default function Admin() {
	document.title = 'Admin';

	const { showToast } = useCustomToast();

	const [file, setFile] = useState<File | null>(null);

	const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		if (event.target.files && event.target.files.length > 0) {
			setFile(event.target.files[0]);
		}
	};

	const uploadEvent = async (values: IEvent) => {
		console.log('values: ', values);
		if (!file) {
			console.error('No file selected');
			return;
		}

		const reader = new FileReader();
		reader.readAsDataURL(file);
		reader.onloadend = async () => {
			const base64data = reader.result as string;

			try {
				const formattedDateOfBirth = dayjs(values.eventDateTime).format(
					'DD-MM-YYYY'
				);

				const userData: IEvent = {
					id: '',
					title: values.title,
					description: values.description,
					location: values.location,
					category: values.category,
					maxParticipants: values.maxParticipants,
					eventDateTime: formattedDateOfBirth,
					participantsCount: values.participantsCount,
					image: base64data.split(',')[1],
				};

				const result = await createEvent(userData);
				console.log('Event created:', result);

				if (result !== null) {
					showToast({
						title: 'Успешно!',
						status: 'success',
					});
				} else if (typeof result === 'string') {
					// Если result — это строка (ошибка), выводим сообщение об ошибке
					showToast({
						title: result,
						status: 'error',
					});
				}
			} catch (error) {
				console.error('Error uploading event:', error);
			}
		};
	};

	return (
		<>
			<Formik
				initialValues={{
					title: '',
					description: '',
					eventDateTime: '',
					location: '',
					category: '',
					maxParticipants: 1,
					participantsCount: 0,
				}}
				validationSchema={EventSchema}
				onSubmit={(values, { setSubmitting }) => {
					const formattedValues = {
						...values,
						dateOfBirth: values.eventDateTime
							? dayjs(values.eventDateTime).format('DD-MM-YYYY')
							: null,
					};
					uploadEvent(formattedValues);
					setSubmitting(false);
				}}
			>
				{({ errors, touched, handleSubmit, isSubmitting, setFieldValue }) => (
					<form onSubmit={handleSubmit} className='flex flex-col gap-1 -10'>
						<Field
							name='title'
							placeholder='Title'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								touched.title && errors.title
									? 'border-red-500'
									: 'border-gray-300'
							)}
						/>
						<ErrorMessage
							name='title'
							component='div'
							className='text-red-500'
						/>

						<Field
							name='description'
							placeholder='Description'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								touched.description && errors.description
									? 'border-red-500'
									: 'border-gray-300'
							)}
						/>
						<ErrorMessage
							name='description'
							component='div'
							className='text-red-500'
						/>

						<Field name='eventDateTime'>
							{({ field }: { field: FieldInputProps<Date | null> }) => (
								<DatePicker
									{...field}
									value={field.value ? dayjs(field.value) : null}
									onChange={date => {
										setFieldValue('eventDateTime', date ? date.toDate() : null);
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
												touched.eventDateTime && !errors.eventDateTime,
										}
									)}
								/>
							)}
						</Field>
						<ErrorMessage
							name='eventDateTime'
							component='div'
							className='text-red-500'
						/>

						<Field
							name='location'
							placeholder='Location'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								touched.location && errors.location
									? 'border-red-500'
									: 'border-gray-300'
							)}
						/>
						<ErrorMessage
							name='location'
							component='div'
							className='text-red-500'
						/>

						<Field
							name='category'
							placeholder='Category'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								touched.category && errors.category
									? 'border-red-500'
									: 'border-gray-300'
							)}
						/>
						<ErrorMessage
							name='category'
							component='div'
							className='text-red-500'
						/>

						<Field
							name='maxParticipants'
							type='number'
							placeholder='Max Participants'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								touched.maxParticipants && errors.maxParticipants
									? 'border-red-500'
									: 'border-gray-300'
							)}
						/>
						<ErrorMessage
							name='maxParticipants'
							component='div'
							className='text-red-500'
						/>

						<input type='file' onChange={handleFileChange} className='mt-2' />

						<Button
							htmlType='submit'
							className='!bg-[#1e293b] mt-3'
							type='primary'
							size='large'
							disabled={isSubmitting}
						>
							Create Event
						</Button>
					</form>
				)}
			</Formik>
		</>
	);
}
