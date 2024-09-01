import { useToast } from '@chakra-ui/react';
import { StatusType } from '../utils/types';

interface ToastProps {
	title: string;
	description?: string;
	status: StatusType;
}

const useCustomToast = () => {
	const toast = useToast();

	const showToast = ({ title, description = '', status }: ToastProps) => {
		toast({
			title,
			description,
			status,
			variant: 'solid',
			isClosable: true,
			position: 'top-right',
		});
	};

	return { showToast };
};

export default useCustomToast;
