import classNames from 'classnames';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { Link } from 'react-router-dom';
import * as Yup from 'yup';
import Button from '../../components/Button';
import FormBlock from '../../components/FormBlock';
import useCustomToast from '../../components/Toast';
import { login } from '../../utils/api/userApi';
import { useUserStore } from '../../utils/store/UserStoreContext';
import { IUser } from '../../utils/types/types';

const LoginSchema = Yup.object().shape({
	password: Yup.string()
		.required('Please Enter your password')
		.matches(
			/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{3,})/,
			'Must Contain 7 Characters, One Uppercase, One Lowercase, One Number'
		),
	email: Yup.string()
		.email('Invalid email')
		.required('Please Enter your Email'),
});
export default function Login() {
	document.title = 'Login';

	const userStore = useUserStore();
	const { setAuth } = userStore;
	
	const { showToast } = useCustomToast();

	const handleLogin = async (values: IUser) => {
		try {
			const result = await login(values);

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
			heading='Войти в аккаунт'
			description='Пожалуйста, войдите в аккаунт'
		>
			<Formik
				initialValues={{
					email: '',
					password: '',
				}}
				validationSchema={LoginSchema}
				onSubmit={(values, { setSubmitting }) => {
					setTimeout(() => {
						setSubmitting(false);
						handleLogin(values);
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
							className='text-red-400 text-base leading-4 text-start pt-1'
						/>

						<Button
							htmlType='submit'
							className='bg-blue-500 mt-3'
							type='primary'
							size='large'
							disabled={isSubmitting}
						>
							Войти в аккаунт
						</Button>

						<Link
							to='/registration'
							className='text-gray-500 mt-3 text-center w-full'
						>
							Зарегистрироваться
						</Link>
					</Form>
				)}
			</Formik>
		</FormBlock>
	);
}
