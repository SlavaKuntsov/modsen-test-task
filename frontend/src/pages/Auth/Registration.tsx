import classNames from 'classnames';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { Link } from 'react-router-dom';
import * as Yup from 'yup';
import Button from '../../components/Button';
import FormBlock from '../../components/FormBlock';

const RegistrationSchema = Yup.object().shape({
	password: Yup.string()
		.required('Please Enter your Password')
		.matches(
			'^(?=.*[A-Za-z])(?=.*d)(?=.*[@$!%*#?&])[A-Za-zd@$!%*#?&]{8,}$',
			'Must Contain 8 Characters, One Uppercase, One Lowercase, One Number and one special case Character'
		),
	confirmPassword: Yup.string()
		.required('Please Enter your Confirm Password')
		.oneOf([Yup.ref('password'), null], 'Passwords must match'),
	email: Yup.string()
		.email('Invalid email')
		.required('Please Enter your Email'),
});

export default function Registration() {
	document.title = 'Registration';

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
						alert(JSON.stringify(values, null, 2));
						setSubmitting(false);
					}, 400);
					console.log(values);
				}}
			>
				{({ errors, touched, handleSubmit, isSubmitting }) => (
					<Form
						onSubmit={handleSubmit}
						className='flex flex-col items-start gap-1'
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
							className='text-red-400 text-base leading-3 text-start pt-1'
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