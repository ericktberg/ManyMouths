import { View, Text } from "react-native";
import { Button } from "@/components/ui/button";
import { useLocalSearchParams, useRouter } from "expo-router";

// This is a simple modal for linking an ingredient to a product or entering a temporary cost
export default function LinkIngredientModal() {
  const { ingredientId } = useLocalSearchParams();
  const router = useRouter();

  // TODO: Fetch ingredient details by ID, and show linking options

  return (
    <View className="flex-1 bg-white p-6 justify-center items-center">
      <Text className="text-xl font-bold mb-4">Link Ingredient</Text>
      <Text className="mb-2">Ingredient ID: {ingredientId}</Text>
      {/* Show ingredient info, product linking UI, and temp cost input here */}
      <Button className="mt-6" onPress={() => {
        if (router.canGoBack()) {
          router.back();
        }
        else {
          router.push('/recipes');
        }
      }}>Close</Button>
    </View>
  );
}
