import { LoadingOutlined } from '@ant-design/icons';
import { Spin } from 'antd';

type LoaderProps = {
	size: 'small' | 'medium' | 'large';
};

export default function Loader({ size }: LoaderProps) {
	// Преобразуем 'medium' в 'default', так как Ant Design не поддерживает 'medium'
	const spinSize = size === 'medium' ? 'default' : size;

	return (
		<Spin
		className='w-full h-full flex items-center justify-center'
			indicator={<LoadingOutlined spin />}
			size={spinSize} // Используем корректный тип 'small' | 'large' | 'default'
		/>
	);
}
