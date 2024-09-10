import { LoadingOutlined } from '@ant-design/icons';
import { Spin } from 'antd';
import classNames from 'classnames';

type LoaderProps = {
	size: 'small' | 'medium' | 'large';
	className?: string;
};

export default function Loader({ size, className }: LoaderProps) {
	const spinSize = size === 'medium' ? 'default' : size;

	return (
		<Spin
			className={classNames(
				'w-full flex items-center justify-center',
				className
			)}
			indicator={<LoadingOutlined spin />}
			size={spinSize} // Используем корректный тип 'small' | 'large' | 'default'
		/>
	);
}
