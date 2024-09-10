import { ButtonProps as AntButtonProps, Button as BaseButton } from 'antd';
import classNames from 'classnames';
import { FC, ReactNode } from 'react';

interface ButtonProps extends AntButtonProps {
	children: ReactNode;
	className?: string;
	size?: 'small' | 'middle' | 'large';
}

const Button: FC<ButtonProps> = ({ children, className, size, ...props }) => (
	<BaseButton
		{...props}
		className={classNames(
			'button',
			className,
			{
				'button--large': size === 'large',
			},
			'bg-blue-500 w-full '
		)}
	>
		{children}
	</BaseButton>
);

export default Button;
