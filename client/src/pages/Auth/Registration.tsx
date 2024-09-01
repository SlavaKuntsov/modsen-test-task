import { DatePicker } from 'antd';
import classNames from 'classnames';
import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
import { ErrorMessage, Field, FieldInputProps, Form, Formik } from 'formik';
import { Link } from 'react-router-dom';
import * as Yup from 'yup';
import Button from '../../components/Button';
import FormBlock from '../../components/FormBlock';
import useCustomToast from '../../components/Toast';
import { registration } from '../../utils/api/userApi';
import { useUserStore } from '../../utils/store/UserStoreContext';
import { IUser } from '../../utils/types';

// Подключаем плагин для поддержки пользовательского формата
dayjs.extend(customParseFormat);

const RegistrationSchema = Yup.object().shape({
	firstName: Yup.string().required('Please Enter your FirstName'),
	lastName: Yup.string().required('Please Enter your LastName'),
	dateOfBirth: Yup.date()
		.required('Please Enter your Date Of Birth')
		.typeError('Invalid date format'),
	password: Yup.string()
		.required('Please Enter your Password')
		.matches(
			/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{3,})/,
			'Must Contain 7 Characters, One Uppercase, One Lowercase, One Number'
		),
	passwordConfirmation: Yup.string()
		.required('Please Enter your Confirm Password')
		.oneOf([Yup.ref('password')], 'Passwords must match'),
	email: Yup.string()
		.email('Invalid email')
		.required('Please Enter your Email'),
});

export default function Registration() {
	document.title = 'Registration';

	const userStore = useUserStore();
	const { setAuth } = userStore;
	const { showToast } = useCustomToast();

	const handleRegistration = async (values: IUser) => {
		try {
			const formattedDateOfBirth = dayjs(values.dateOfBirth).format('DD-MM-YYYY');

			const userData: IUser = {
				firstName: values.firstName,
				lastName: values.lastName,
				dateOfBirth:  formattedDateOfBirth,
				email: values.email,
				password: values.password,
				passwordConfirmation: values.passwordConfirmation,
				role: 'User'
			};
	
			const result = await registration(userData); // Используем новый объект

			if (result === true) {
				showToast({
					title: 'Успешно!',
					status: 'success',
				});
				setAuth(true);
			} else if (typeof result === 'string') {
				showToast({
					title: result,
					status: 'error',
				});
			}
		} catch (error) {
			console.error('Error creating user:', error);
			throw error;
		}
	};

	return (
		<FormBlock
			heading='Регистрация'
			description='Для входа вам нужно зарегистрироваться'
		>
			<Formik
				initialValues={{
					firstName: '',
					lastName: '',
					dateOfBirth: null, // Изменяем значение на null
					email: '',
					password: '',
					passwordConfirmation: '',
				}}
				validationSchema={RegistrationSchema}
				onSubmit={(values, { setSubmitting }) => {
					handleRegistration(values);
					setSubmitting(false);
				}}
			>
				{({ errors, touched, handleSubmit, isSubmitting, setFieldValue }) => (
					<Form
						onSubmit={handleSubmit}
						className='flex flex-col items-start gap-1 max-w-80'
					>
						{/* FirstName Field */}
						<Field
							placeholder='FirstName'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								{
									'border-red-500': touched.firstName && errors.firstName,
									'border-green-500': touched.firstName && !errors.firstName,
								}
							)}
							name='firstName'
						/>
						<ErrorMessage
							name='firstName'
							component='span'
							className='text-red-400 text-base leading-3 text-start pt-1'
						/>

						{/* LastName Field */}
						<Field
							placeholder='LastName'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								{
									'border-red-500': touched.lastName && errors.lastName,
									'border-green-500': touched.lastName && !errors.lastName,
								}
							)}
							name='lastName'
						/>
						<ErrorMessage
							name='lastName'
							component='span'
							className='text-red-400 text-base leading-3 text-start pt-1'
						/>

						{/* Email Field */}
						<Field
							type='email'
							placeholder='Email'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								{
									'border-red-500': touched.email && errors.email,
									'border-green-500': touched.email && !errors.email,
								}
							)}
							name='email'
						/>
						<ErrorMessage
							name='email'
							component='span'
							className='text-red-400 text-base leading-3 text-start pt-1'
						/>

						{/* Password Field */}
						<Field
							type='password'
							placeholder='Password'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								{
									'border-red-500': touched.password && errors.password,
									'border-green-500': touched.password && !errors.password,
								}
							)}
							name='password'
						/>
						<ErrorMessage
							name='password'
							component='span'
							className='text-red-400 text-base leading-3 text-start pt-1'
						/>

						{/* Confirm Password Field */}
						<Field
							type='password'
							placeholder='Confirm Password'
							className={classNames(
								'w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								{
									'border-red-500':
										touched.passwordConfirmation && errors.passwordConfirmation,
									'border-green-500':
										touched.passwordConfirmation &&
										!errors.passwordConfirmation,
								}
							)}
							name='passwordConfirmation'
						/>
						<ErrorMessage
							name='passwordConfirmation'
							component='span'
							className='text-red-400 text-base leading-4 text-start pt-1'
						/>

						{/* Date of Birth Field */}
						<Field name='dateOfBirth'>
							{({ field }: { field: FieldInputProps<Date | null> }) => (
								<DatePicker
									{...field}
									value={field.value ? dayjs(field.value) : null}
									onChange={date => {
										// Проверяем, если date существует и преобразуем его в объект Date
										setFieldValue('dateOfBirth', date ? date.toDate() : null);
										console.log(date.toDate())
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
							className='text-red-400 text-base leading-4 text-start pt-1'
						/>

						{/* Submit Button */}
						<Button
							htmlType='submit'
							className='bg-blue-500 mt-3'
							type='primary'
							size='large'
							disabled={isSubmitting}
						>
							Зарегистрироваться
						</Button>

						<Link to='/login' className='text-gray-500 mt-3 text-center w-full'>
							Войти в аккаунт
						</Link>
					</Form>
				)}
			</Formik>
		</FormBlock>
	);
}
