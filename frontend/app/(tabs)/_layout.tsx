import { Tabs } from 'expo-router';

export default function TabLayout() {
    return (
        <Tabs screenOptions={{ headerShown: false }}>
            <Tabs.Screen
                name="index"
                options={{
                    title: "Hello"
                }} />
            <Tabs.Screen
                name="grocerylist/index"
                options={{
                    title: 'Groceries'
                }} />
        </Tabs>
    );
}