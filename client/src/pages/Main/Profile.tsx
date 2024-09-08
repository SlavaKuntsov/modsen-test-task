import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
import * as Yup from 'yup';
import useCustomToast from '../../components/Toast';
import { updateParticipant } from '../../utils/api/userApi';
import { userStore } from '../../utils/store/userStore';
import { IUserUpdate } from '../../utils/types';

dayjs.extend(customParseFormat);

const UserProfileSchema = Yup.object().shape({
	firstName: Yup.string().required('Please Enter your FirstName'),
	lastName: Yup.string().required('Please Enter your LastName'),
	dateOfBirth: Yup.date()
		.required('Please Enter your Date Of Birth')
		.typeError('Invalid date format'),
});

export default function Profile() {
	document.title = 'Profile';

	const { setAuth, user, setUser } = userStore;

	const { showToast } = useCustomToast();

	const handleUpdate = async (values: IUserUpdate) => {
		console.log('values: ', values);

		try {
			// const formattedDateOfBirth = dayjs(values.dateOfBirth).format(
			// 	'DD-MM-YYYY'
			// );

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
				setUser(result); // Устанавливаем пользователя только если это объект
				showToast({
					title: 'Успешно!',
					status: 'success',
				});
				setAuth(true);
			} else if (typeof result === 'string') {
				// Если result — это строка (ошибка), выводим сообщение об ошибке
				showToast({
					title: result,
					status: 'error',
				});
			}
		} catch (error) {
			console.error('Error updating user:', error);
			setAuth(false);
			showToast({
				title: 'An unexpected error occurred',
				status: 'error',
			});
		}
	};

	return (
		<>
			<h1 className='text-2xl font-semibold'>Ваш Профиль</h1>
		</>
	);
}
