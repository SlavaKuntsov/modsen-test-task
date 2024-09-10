import classNames from 'classnames';
import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
import { eventStore } from '../../utils/store/eventsStore';
import { IEvent } from './../../utils/types';

dayjs.extend(customParseFormat);
interface Props {
	item: IEvent;
}

export default function EventListItem({ item }: Props) {
	const { selectedEvent, setSelectEvent } = eventStore;

	return (
		<button
			onClick={() => setSelectEvent(item.id)}
			className={classNames(
				'ease-in-out px-5 py-4 w-full text-start',
				{
					'bg-zinc-100': selectedEvent == item.id,
				}
			)}
		>
			<div className='flex flex-row items-center justify-between'>
				<h3 className='text-nowrap text-lg font-medium'>{item.title}</h3>
				<p>{dayjs(item.eventDateTime).format('DD-MM-YYYY')}</p>
			</div>
			<p>{item.description}</p>
		</button>
	);
}
