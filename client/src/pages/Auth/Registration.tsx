import classNames from 'classnames';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { Link } from 'react-router-dom';
import * as Yup from 'yup';
import Button from '../../components/Button';
import FormBlock from '../../components/FormBlock';
import useCustomToast from '../../components/Toast';
import { registration } from '../../utils/api/userApi';
import { useUserStore } from '../../utils/store/UserStoreContext';
import { IUser } from '../../utils/types/types';

const RegistrationSchema = Yup.object().shape({
	password: Yup.string()
		.required('Please Enter your Password')
		.matches(
			/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{3,})/,
			'Must Contain 7 Characters, One Uppercase, One Lowercase, One Number'
		),
	confirmPassword: Yup.string()
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
			const result = await registration(values);

			if (result === true) {
				showToast({
					title: 'Успешно!', // Используем строку в качестве заголовка
					status: 'success', // В данном случае ошибка
				});
				setAuth(true);
			} else if (typeof result === 'string') {
				// Если результат - это строка, отображаем её с помощью Toast
				showToast({
					title: result, // Используем строку в качестве заголовка
					status: 'error', // В данном случае ошибка
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
					email: '',
					password: '',
					confirmPassword: '',
				}}
				validationSchema={RegistrationSchema}
				onSubmit={(values, { setSubmitting }) => {
					setTimeout(() => {
						handleRegistration(values);
						setSubmitting(false);
					}, 400);
					console.log(values);
				}}
			>
				{({ errors, touched, handleSubmit, isSubmitting }) => (
					<Form
						onSubmit={handleSubmit}
						className='flex flex-col items-start gap-1 max-w-80'
					>
						<Field
							placeholder='Еmail'
							className={classNames(
								' w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								{
									'border-red-500': touched.email && errors.email,
								},
								{
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

						<Field
							placeholder='Password'
							className={classNames(
								' w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								{
									'border-red-500': touched.password && errors.password,
								},
								{
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

						<Field
							placeholder='Confirm Password'
							className={classNames(
								' w-80 py-3 px-5 mt-2 border-2 border-gray-200 border-solid rounded transition-[border] ease-in-out duration-500',
								{
									'border-red-500':
										touched.confirmPassword && errors.confirmPassword,
								},
								{
									'border-green-500':
										touched.confirmPassword && !errors.confirmPassword,
								}
							)}
							name='confirmPassword'
						/>
						<ErrorMessage
							name='confirmPassword'
							component='span'
							className='text-red-400 text-base leading-4 text-start pt-1'
						/>

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
