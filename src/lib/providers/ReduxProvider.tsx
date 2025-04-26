'use client';

import { Provider } from 'react-redux';
import  store  from '@/lib/store/store'; // adjust the import path as needed

export default function ReduxProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  return <Provider store={store}>{children}</Provider>;
}
