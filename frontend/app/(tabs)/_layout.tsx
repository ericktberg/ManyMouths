// app/_layout.tsx
import { Tabs } from "expo-router";
import { ShoppingCart, BookIcon, LucideIcon } from "lucide-react-native";
import { Text, View, Animated } from "react-native";
import { appColors } from "@/colors";

function TabBarButton({
    title,
    Icon,
    focused,
    activeColor,
}: {
    title: string;
    Icon: LucideIcon;
    focused: boolean;
    activeColor: string;
}) {
    return (
        <View
            className="items-center px-3 py-2 rounded-lg min-h-[48px] min-w-[48px]"
        >
            <Icon
                size={22}
                color={focused ? activeColor : appColors.textdisabledforeground}
            />
            <Text
                className={`text-xs ${focused ? `font-semibold` : ""}`}
                style={{
                    color: focused ? activeColor : appColors.textdisabledforeground,
                }}
            >
                {title}
            </Text>
        </View>
    );
}

// Your layout function
export default function Layout() {
    return (
        <Tabs
            screenOptions={{
                headerShown: false,
                tabBarShowLabel: false,
            }}
        >
            <Tabs.Screen
                name="groceries"
                options={{
                    title: "Groceries",
                    tabBarIcon: ({ focused }) => (
                        <TabBarButton
                            title="Groceries"
                            Icon={ShoppingCart}
                            focused={focused}
                            activeColor={appColors.blueberry}
                        />
                    ),
                }}
            />

            <Tabs.Screen
                name="recipes"
                options={{
                    title: "Recipes",
                    tabBarIcon: ({ focused }) => (
                        <TabBarButton
                            title="Recipes"
                            Icon={BookIcon}
                            focused={focused}
                            activeColor={appColors.grape}
                        />
                    ),
                }}
            />
        </Tabs>
    );
}