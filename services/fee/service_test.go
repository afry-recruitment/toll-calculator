package fee

import (
	"errors"
	"reflect"
	"testing"
	"time"

	mock_dagsmart "afry-toll-calculator/mocks/afry-toll-calculator/integrations/dagsmart"
	mock_pricelist "afry-toll-calculator/mocks/afry-toll-calculator/services/pricelist"
	mock_vehiclelist "afry-toll-calculator/mocks/afry-toll-calculator/services/vehiclelist"
	"afry-toll-calculator/models"
	"github.com/stretchr/testify/mock"
)

func Test_feeService_getHolidays(t *testing.T) {
	tests := []struct {
		name        string
		mocks       func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService)
		vehicleType models.VehicleType
		entryDates  []time.Time
		want        int
		wantErr     bool
		wantErrText string
	}{
		{
			name: "a toll free vehicle pays nothing",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 10, 0, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 20, 0, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("motorbike"),
			want:        0,
		},
		{
			name: "normal vehicle pays two entry fees, for two separate blocks",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
				dagsmart.EXPECT().Get(mock.Anything).Return([]string{}, nil)

				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 10, 0, 0, 0, time.UTC)).Return(5)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 20, 0, 0, 0, time.UTC)).Return(6)
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 10, 0, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 20, 0, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			want:        11,
		},
		{
			name: "normal vehicle pays two entry fees, for two separate blocks, despite entering each block 3 times",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
				dagsmart.EXPECT().Get(mock.Anything).Return([]string{}, nil)

				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 10, 0, 0, 0, time.UTC)).Return(5)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 10, 15, 0, 0, time.UTC)).Return(5)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 10, 26, 0, 0, time.UTC)).Return(5)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 20, 11, 0, 0, time.UTC)).Return(6)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 20, 15, 0, 0, time.UTC)).Return(6)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 20, 20, 0, 0, time.UTC)).Return(6)
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 10, 0, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 10, 15, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 10, 26, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 20, 11, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 20, 15, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 20, 20, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			want:        11,
		},
		{
			name: "normal vehicle pays two entry fees, because third entry misses last block by a minute",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
				dagsmart.EXPECT().Get(mock.Anything).Return([]string{}, nil)

				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 10, 0, 0, 0, time.UTC)).Return(5)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 10, 15, 0, 0, time.UTC)).Return(5)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 11, 00, 0, 0, time.UTC)).Return(5)
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 10, 0, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 10, 15, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 11, 0, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			want:        10,
		},
		{
			name: "normal vehicle pays the highest fee in each block",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
				dagsmart.EXPECT().Get(mock.Anything).Return([]string{}, nil)

				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 10, 00, 0, 0, time.UTC)).Return(3)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 10, 15, 0, 0, time.UTC)).Return(8)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 11, 00, 0, 0, time.UTC)).Return(6)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 11, 05, 0, 0, time.UTC)).Return(11)
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 10, 00, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 10, 15, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 11, 00, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 11, 05, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			want:        19,
		},
		{
			name: "normal vehicle exceeds maximum daily toll fee and pays the maximum daily rate",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
				dagsmart.EXPECT().Get(mock.Anything).Return([]string{"2020-03-05"}, nil)

				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 01, 00, 0, 0, time.UTC)).Return(18)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 06, 00, 0, 0, time.UTC)).Return(25)
				pricelist.EXPECT().GetPrice(time.Date(2020, 1, 1, 11, 05, 0, 0, time.UTC)).Return(33)
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 01, 00, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 06, 00, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 11, 05, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			want:        60,
		},
		{
			name: "normal vehicle has free pass on saturday",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
			},
			entryDates: []time.Time{
				time.Date(2025, 12, 6, 01, 00, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			want:        0,
		},
		{
			name: "normal vehicle has free pass on sunday",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
			},
			entryDates: []time.Time{
				time.Date(2025, 12, 7, 01, 00, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			want:        0,
		},
		{
			name: "normal vehicle has free pass on holidays",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
				dagsmart.EXPECT().Get(mock.Anything).Return([]string{"2020-01-01"}, nil)
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 01, 00, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 06, 00, 0, 0, time.UTC),
				time.Date(2020, 1, 1, 11, 05, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			want:        0,
		},
		{
			name: "GetFee expects all entry times on the same day",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 01, 00, 0, 0, time.UTC),
				time.Date(2020, 1, 2, 06, 00, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			wantErr:     true,
			wantErrText: "GetFee call contains more than one day of entry times",
		},
		{
			name: "holiday API returns an error",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
				dagsmart.EXPECT().Get(mock.Anything).Return(nil, errors.New("some error"))
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 01, 00, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("car"),
			wantErr:     true,
			wantErrText: "some error",
		},
		{
			name: "invalid vehicle type",
			mocks: func(getter *mock_vehiclelist.MockGetter, dagsmart *mock_dagsmart.MockService, pricelist *mock_pricelist.MockService) {
			},
			entryDates: []time.Time{
				time.Date(2020, 1, 1, 01, 00, 0, 0, time.UTC),
			},
			vehicleType: models.VehicleType("foo"),
			wantErr:     true,
			wantErrText: "unknown vehicle type",
		},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			mockVehicleListGetter := mock_vehiclelist.NewMockGetter(t)
			mockDagsmartService := mock_dagsmart.NewMockService(t)
			mockpriceListService := mock_pricelist.NewMockService(t)

			tt.mocks(mockVehicleListGetter, mockDagsmartService, mockpriceListService)
			mockVehicleListGetter.EXPECT().GetVehicleList().Return([]models.Vehicle{
				models.NewVehicle("car", false),
				models.NewVehicle("motorbike", true),
			})

			s := New(
				mockVehicleListGetter,
				mockDagsmartService,
				mockpriceListService,
			)

			got, err := s.GetFee(tt.vehicleType, tt.entryDates)
			if (err != nil) != tt.wantErr {
				t.Errorf("getFee() error = %v, wantErr %v", err, tt.wantErr)
				return
			}
			if tt.wantErr && err.Error() != tt.wantErrText {
				t.Errorf("Get() error = %v, wantErrText %v", err, tt.wantErrText)
				return
			}
			if !reflect.DeepEqual(got, tt.want) {
				t.Errorf("getFee() got = %v, want %v", got, tt.want)
			}
		})
	}
}
