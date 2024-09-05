import classNames from 'classnames';
import { eventStore } from '../../utils/store/eventsStore';
import { IEvent } from './../../utils/types';

interface Props {
	item: IEvent;
}

export default function EventListItem({ item }: Props) {
	const { selectedEvent, setSelectEvent } = eventStore;

	return (
		<button
			onClick={() => setSelectEvent(item.id)}
			className={classNames('px-5 py-4 min-w-80 max-w-10 text-start', {
				'bg-zinc-100': selectedEvent == item.id,
			})}
		>
			<h3 className='text-nowrap text-lg font-medium'>{item.title}</h3>
			<p>{item.description}</p>
		</button>
	);
}
