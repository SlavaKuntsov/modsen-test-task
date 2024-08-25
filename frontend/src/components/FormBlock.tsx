import { ReactNode } from 'react';

type Props = {
	children: ReactNode;
	heading: string;
	description: string;
};

export default function FormBlock({ children, heading, description }: Props) {
	return (
		<section className='text-center p-5 my-auto'>
			<div className='mb-10'>
				<h1 className='text-slate-950 font-semibold text-3xl pb-3'>
					{heading}
				</h1>
				<h3 className='text-gray-500 text-lg'>{description}</h3>
			</div>
			<div className='shadow-xl p-12 mb-24'>{children}</div>
		</section>
	);
}
