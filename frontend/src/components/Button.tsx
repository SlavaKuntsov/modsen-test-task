import { Button as BaseButton } from 'antd';
import classNames from 'classnames';

const Button = ({ children, className, size, ...props }) => (
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
