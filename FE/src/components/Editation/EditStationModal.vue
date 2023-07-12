<template>
  <v-form>
    <v-container>
      {{ station }}
      <v-row>
        <v-col cols="12" md="12">
          <ValidationProvider v-slot="{ errors }" rules="required" :name="$t('general.name')">
            <v-text-field
              :label="$t('general.name')"
              required
              :value="station.name"
              :error-messages="errors"
              @input="$emit('update:station', { ...station, name: $event })"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="12">
          <ValidationProvider v-slot="{ errors }" rules="coordinates_validation">
            <v-text-field
              :label="$t('stations.position')"
              :value="positionValue"
              :error-messages="errors"
              @input="$emit('update:station', { ...station, ...getLatLong($event) })"
              @keypress="justNumber"
            />
          </ValidationProvider>
        </v-col>
      </v-row>
    </v-container>
  </v-form>
</template>

<script>
import { ValidationProvider } from "vee-validate";
import { getLatLong, getPositionValue, justNumber } from "../../code/helpers/positionHelpers";

export default {
  name: "EditStationModal",
  components: {
    ValidationProvider,
  },

  props: {
    station: {
      type: Object,
      required: true,
    },
  },
  computed: {
    positionValue() {
      return getPositionValue(this.station);
    },
  },

  methods: {
    getLatLong,
    justNumber,
  },
};
</script>
