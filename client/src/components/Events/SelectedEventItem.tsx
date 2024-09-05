import { IEvent } from '../../utils/types';

type SelectedEventItemProps = {
	item: IEvent;
};

export default function SelectedEventItem({ item }: SelectedEventItemProps) {
	return (
		<div className='w-full h-full'>
			<div className='relative'>
				<img
					className='w-full h-80 border-2 border-solid border-red-200'
					src=''
					alt='event image'
				/>
				<h1 className='px-5 py-3 bg-zinc-50 rounded-md absolute left-24 bottom-4 font-semibold text-2xl'>
					{item.title}
				</h1>
			</div>
			<div className='px-8 py-3'>
				<h3 className='text-lg'>Описание: {item.description}</h3>
			</div>
		</div>
	);
}
