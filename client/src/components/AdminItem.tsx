import classNames from 'classnames';
import { observer } from 'mobx-react-lite';
import { activateAdmin, deactivateAdmin } from '../utils/api/userApi';
import { IAdmin, IDelete } from '../utils/types';
import Button from './Button';
import useCustomToast from './Toast';

interface AdminItemProps {
	item: IAdmin;
	refreshAdmins: () => Promise<void>;
}

const AdminItem = observer(({ item, refreshAdmins }: AdminItemProps) => {
	const { showToast } = useCustomToast();

	const handleActivation = async () => {
		try {
			const eventData: IDelete = {
				id: item?.id,
			};

			const result = await activateAdmin(eventData);

			if (result === true) {
				refreshAdmins();
				showToast({
					title: 'Успешно!',
					description: 'Администратор активирован!',
					status: 'success',
				});
			} else if (typeof result === 'string') {
				showToast({
					title: result,
					status: 'error',
				});
			}
		} catch (error) {
			console.error('Error on activate admin:', error);
			throw error;
		}
	};

	const handleDeactivation = async () => {
		try {
			const eventData: IDelete = {
				id: item?.id,
			};

			const result = await deactivateAdmin(eventData);

			if (result === true) {
				refreshAdmins();
				showToast({
					title: 'Успешно!',
					description: 'Администратор деактивирован!',
					status: 'success',
				});
			} else if (typeof result === 'string') {
				showToast({
					title: result,
					status: 'error',
				});
			}
		} catch (error) {
			console.error('Error on deactivate admin:', error);
			throw error;
		}
	};

	return (
		<div className='flex flex-row gap-10 items-center justify-center'>
			<h3
				className={classNames(
					'text-xl font-medium',
					{ 'text-green-700': item.isActiveAdmin },
					{ 'text-red-500': !item.isActiveAdmin }
				)}
			>
				{item.email}
			</h3>
			{!item.isActiveAdmin ? (
				<Button
					onClick={handleActivation}
					className='!bg-[#1e293b]'
					type='primary'
					size='middle'
				>
					Активировать
				</Button>
			) : (
				<Button
					onClick={handleDeactivation}
					className='!bg-[#1e293b]'
					type='primary'
					size='middle'
				>
					Деактивировать
				</Button>
			)}
		</div>
	);
});

export default AdminItem;
