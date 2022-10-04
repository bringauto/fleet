<template>
  <v-form>
    <v-container>
      <v-row>
        <v-col cols="12" md="6">
          <ValidationProvider v-slot="{ errors }" rules="required" :name="$t('general.name')">
            <v-text-field
              :label="$t('general.name')"
              required
              :value="car.name"
              :error-messages="errors"
              @input="$emit('update:car', { ...car, name: $event })"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="6">
          <v-text-field
            :label="$t('cars.hwId')"
            :value="car.hwId"
            @input="$emit('update:car', { ...car, hwId: $event })"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-text-field
            :label="$t('cars.companyName')"
            :value="car.companyName"
            @input="$emit('update:car', { ...car, companyName: $event })"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-text-field
            :label="$t('cars.carAdminPhone')"
            :value="car.carAdminPhone"
            @input="$emit('update:car', { ...car, carAdminPhone: $event })"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-select
            :items="routes"
            :label="$t('settings.route')"
            :value="car.routeId"
            hide-details
            clearable
            item-text="name"
            item-value="id"
            @input="$emit('update:car', { ...car, routeId: $event })"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-select
            :value="car.status"
            :items="CarStateFormated"
            item-text="trans"
            item-value="status"
            :label="$t('general.status')"
            hide-details
            @input="$emit('update:car', { ...car, status: $event })"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-checkbox
            :input-value="car.underTest"
            :false-value="false"
            :true-value="true"
            :label="$t('cars.underTest')"
            @change="$emit('update:car', { ...car, underTest: $event })"
          />
        </v-col>
      </v-row>
    </v-container>
  </v-form>
</template>

<script>
import { ValidationProvider } from "vee-validate";
import { CarStateFormated } from "../../code/enums/carEnums";

export default {
  name: "EditStationModal",
  components: {
    ValidationProvider,
  },
  props: {
    car: {
      type: Object,
      required: true,
    },
    routes: {
      type: Array,
      default: () => {
        return [];
      },
    },
  },
  data: () => ({
    CarStateFormated,
  }),
};
</script>
