import classNames from 'classnames';
import { ReactNode } from 'react';
import { Outlet } from 'react-router-dom';
import Nav from '../components/Nav/Nav';

type LayoutContainerProps = {
	children?: ReactNode;
	isAuth?: boolean;
};

export default function LayoutContainer({
	children,
	isAuth = false,
}: LayoutContainerProps) {
	return (
		<div
			className={classNames(
				'container mx-auto flex flex-col items-center h-full w-full',
				{ 'justify-center': isAuth },
				{ 'justify-start ': isAuth }
			)}
		>
			{!isAuth ? (
				<>
					<Nav />
					<main className='h-full w-full'>{children || <Outlet />}</main>
				</>
			) : (
				children || <Outlet />
			)}
		</div>
	);
}
