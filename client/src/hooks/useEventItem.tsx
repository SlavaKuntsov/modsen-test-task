import { useQuery } from '@tanstack/react-query';
import { getEvent } from '../utils/api/eventsApi';
import { IEvent } from '../utils/types';

export function useEventItem(id: string | null) {
	const { isInitialLoading, error, data } = useQuery<IEvent | undefined, Error>(
		{
			queryKey: ['event', id], // Уникальный ключ запроса для каждого id
			queryFn: async () => {
				if (!id || id == null) return undefined;
				console.log('useEventItem ' + id);
				return await getEvent(id);
			},
			staleTime: 1000 * 60 * 60,
			refetchOnWindowFocus: false,
			retry: false,
			enabled: Boolean(id), // Активируем только если есть id
		}
	);

	return {
		eventItem: data,
		isLoading: isInitialLoading,
		error: error,
	};
}
