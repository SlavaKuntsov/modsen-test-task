import { QuestionCircleOutlined } from '@ant-design/icons';
import { Button, DatePicker, Popconfirm } from 'antd';
import classNames from 'classnames';
import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
import { ErrorMessage, Field, FieldInputProps, Formik } from 'formik';
import * as Yup from 'yup';
import useCustomToast from '../../components/Toast';
import { deleteUser, updateParticipant } from '../../utils/api/userApi';
import { eventStore } from '../../utils/store/eventsStore';
import { userStore } from '../../utils/store/userStore';
import { IDelete, IUserUpdate } from '../../utils/types';

dayjs.extend(customParseFormat);

const UserProfileSchema = Yup.object().shape({
	firstName: Yup.string().required('Please Enter your FirstName'),
	lastName: Yup.string().required('Please Enter your LastName'),
	dateOfBirth: Yup.string().required('Event date and time are required'),
});

export default function Profile() {
	document.title = 'Profile';

	const { setAuth, user, setUser, logout } = userStore;

	const { resetStore } = eventStore;

	const { showToast } = useCustomToast();

	const handleUpdate = async (values: IUserUpdate) => {
		console.log('values: ', values);

		try {
			const userData: IUserUpdate = {
				id: user?.id,
				firstName: values.firstName,
				lastName: values.lastName,
				dateOfBirth: values.dateOfBirth,
			};

			console.log('userData: ', userData);
			const result = await updateParticipant(userData);
			console.log('result: ', result);

			// Проверяем, что result является объектом IUser
			if (result !== null) {
				setUser(result);
				showToast({
					title: 'Успешно!',
					status: 'success',
				});
				setAuth(true);
			} else if (typeof result === 'string') {
				// Если result — это строка (ошибка), выводим сообщение об ошибке
				showToast({
					title: 'Ошибка!',
					status: 'error',
					description: result
				});
			}
		} catch (error) {
			console.error('Error updating user:', error);
			setAuth(false);
			showToast({
				title: 'Ошибка!',
				description: 'An unexpected error occurred',
				status: 'error',
			});
		}
	};

	const handleDelete = async () => {
		try {
			const userData: IDelete = {
				id: user?.id,
			};

			const result = await deleteUser(userData);

			// Проверяем, что result является объектом IUser
			if (result === true) {
				await logout();
				resetStore();
				showToast({
					title: 'Успешно!',
					status: 'success',
				});
			} else if (typeof result === 'string') {
				// Если result — это строка (ошибка), выводим сообщение об ошибке
				showToast({
					title: 'Ошибка!',
					status: 'error',
					description: result
				});
			}
		} catch (error) {
			console.error('Error deleting user:', error);
			setAuth(false);
			showToast({
				title: 'Ошибка!',
				description: 'An unexpected error occurred',
				status: 'error',
			});
		}
	};

	return (
		<>
			<h1 className='text-2xl font-semibold mt-5'>Ваш Профиль</h1>
			<Formik
				initialValues={{
					firstName: user?.firstName,
					lastName: user?.lastName,
					dateOfBirth: user?.dateOfBirth,
				}}
				validationSchema={UserProfileSchema}
				onSubmit={(values, { setSubmitting }) => {
					handleUpdate(values);
					setSubmitting(false);
				}}
			>
				{({ errors, touched, handleSubmit, isSubmitting, setFieldValue }) => (
					<form onSubmit={handleSubmit} className='flex flex-col gap-1 -10'>
						<Field
							name='firstName'
							placeholder='FirstName'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								touched.firstName && errors.firstName
									? 'border-red-500'
									: 'border-gray-300'
							)}
						/>
						<ErrorMessage
							name='title'
							component='span'
							className='text-red-500'
						/>

						<Field
							name='lastName'
							placeholder='LastName'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								touched.lastName && errors.lastName
									? 'border-red-500'
									: 'border-gray-300'
							)}
						/>
						<ErrorMessage
							name='lastName'
							component='span'
							className='text-red-500'
						/>

						<Field name='dateOfBirth'>
							{({ field }: { field: FieldInputProps<Date | null> }) => (
								<DatePicker
									{...field}
									value={field.value ? dayjs(field.value, 'DD-MM-YYYY') : null}
									onChange={date => {
										setFieldValue(
											'dateOfBirth',
											date ? date.format('DD-MM-YYYY') : null
										);
										console.log(date ? date.toDate() : null);
									}}
									format='DD-MM-YYYY'
									size='middle'
									className={classNames(
										'w-full h-[39px] mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
										{
											'border-red-500':
												touched.dateOfBirth && errors.dateOfBirth,
											'border-green-500':
												touched.dateOfBirth && !errors.dateOfBirth,
										}
									)}
								/>
							)}
						</Field>

						<ErrorMessage
							name='dateOfBirth'
							component='span'
							className='text-red-500'
						/>

						<Button
							htmlType='submit'
							className='!bg-[#1e293b] mt-3'
							type='primary'
							size='middle'
							disabled={isSubmitting}
						>
							Сохранить профиль
						</Button>

						<Popconfirm
							onConfirm={handleDelete}
							title='Удаление аккаунт'
							description='Вы уверены что хотите удалить аккаунт?'
							icon={<QuestionCircleOutlined style={{ color: 'red' }} />}
						>
							<Button className='!bg-red-500 mt-3' type='primary'>
								Удалить
							</Button>
						</Popconfirm>
					</form>
				)}
			</Formik>
		</>
	);
}
