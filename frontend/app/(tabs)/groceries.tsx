import React from "react";
import { View, Text } from "react-native";

export function ListsView() {
  return (
    <View className="max-w-md mx-auto p-4">
      {/* Header */}
      <View className="mb-6">
        <Text className="text-3xl font-bold bg-gradient-to-r from-blueberry to-eggplant bg-clip-text text-transparent">
          Grocery Lists
        </Text>
        <Text className="text-sm text-muted-foreground mt-1">
          Organized shopping made easy
        </Text>
      </View>

      {/* Empty state */}
      <View className="items-center py-12">
        <Text className="text-6xl mb-4">🛒</Text>
        <Text className="text-muted-foreground font-medium mb-2">
          No grocery lists yet
        </Text>
        <Text className="text-sm text-muted-foreground">
          Long press on recipes to add ingredients to lists
        </Text>
      </View>
    </View>
  );
}

export default function Index() {
  return (
    <ListsView />
  );
}