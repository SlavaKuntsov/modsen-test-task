import { ReactNode } from 'react';
import { Outlet } from 'react-router-dom';

type LayoutProps = {
	children?: ReactNode;
};

export default function Layout({ children }: LayoutProps) {
	return (
		<div className='bg-slate-50 h-full px-2 py-2'>{children || <Outlet />}</div>
	);
}
